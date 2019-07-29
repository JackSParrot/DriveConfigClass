using System;
using System.Globalization;
using System.Collections.Generic;

namespace JackSParrot.JSON
{
    public class MalformedJSONException : Exception
    {
        public MalformedJSONException(string message) : base(message) { }
    }
    public class JSONParser
    {
        const char kOpenObjectToken = '{';
        const char kCloseObjectToken = '}';
        const char kOpenArrayToken = '[';
        const char kCloseArrayToken = ']';
        const char kColonToken = ':';
        const char kCommaToken = ',';
        const char kQuoteToken = '"';

        const char kSpace = ' ';
        const char kLineEnd = '\n';
        const char kLineReturn = '\r';
        const char kTab = '\t';

        const string kTrue = "true";
        const string kTrueCap = "True";
        const string kFalse = "false";
        const string kFalseCap = "False";
        const string kNull = "null";
        const char kMinus = '-';
        const char kPlus = '+';
        const char kZero = '0';
        const char kNine = '9';
        const char kDot = '.';

        char _currentToken;
        int _currentPosition;
        string _serialized;
        int _prevPosition;
        int _nextTokenIndex;

        HashSet<char> _objectTokens = new HashSet<char> {
            kOpenObjectToken,
            kCloseObjectToken,
            kOpenArrayToken,
            kCloseArrayToken,
            kColonToken,
            kCommaToken,
            kQuoteToken
        };

        HashSet<char> _allTokens = new HashSet<char> {
            kOpenObjectToken,
            kCloseObjectToken,
            kOpenArrayToken,
            kCloseArrayToken,
            kColonToken,
            kCommaToken,
            kQuoteToken,
            kPlus,
            kMinus,
            kTrue[0],
            kTrueCap[0],
            kFalse[0],
            kFalseCap[0],
            kNull[0]
        };

        Stack<JSON> _stack = new Stack<JSON>();
        JSON _currentObject = null;

        public JSON Parse(string serialized)
        {
            if(string.IsNullOrEmpty(serialized))
            {
                return null;
            }
            _currentToken = kSpace;
            _currentPosition = -1;
            _prevPosition = -1;

            _serialized = serialized;
            _currentPosition = SearchNextOpenToken();
            _currentToken = _serialized[_currentPosition];
            if(serialized.Length < 1 || _currentPosition < 0)
            {
                throw new MalformedJSONException("Trying to parse an empty or malformed json");
            }
            JSON retVal = null;
            do
            {
                switch(_currentToken)
                {
                    case kOpenObjectToken:
                        {
                            CreateObject(JSONType.Object, string.Empty);
                        }
                    break;
                    case kOpenArrayToken:
                        {
                            CreateObject(JSONType.Array, string.Empty);
                        }
                    break;
                    case kCloseArrayToken:
                    case kCloseObjectToken:
                       {
                            retVal = _stack.Pop();
                            _currentObject = _stack.Count > 0 ? _stack.Peek() : null;
                            if(_stack.Count > 0)
                            {
                                MoveToNextNotEmptyPosition();
                            }
                        }
                    break;
                    case kCommaToken:
                        {
                            MoveToNextNotEmptyPosition();
                        }
                    break;
                    default:
                        {
                            if(_currentObject.GetJSONType() == JSONType.Object)
                            {
                                string key = ParseString();
                                char currentToken = MoveToNextNotEmptyPosition();
                                if(currentToken == kOpenObjectToken || currentToken == kOpenArrayToken)
                                {
                                    CreateObject(currentToken == kOpenObjectToken ? JSONType.Object : JSONType.Array, key);
                                }
                                else
                                {
                                    _currentObject.AsObject().Add(key, ParseValue());
                                }
                            }
                            else if(_currentObject.GetJSONType() == JSONType.Array)
                            {
                                if(_serialized[_currentPosition] == kColonToken)
                                {
                                    throw new MalformedJSONException("Malformed Array");
                                }
                                _currentObject.AsArray().Add(ParseValue());
                            }
                        }
                    break;
                }
            } while(_currentPosition < _serialized.Length && _stack.Count > 0);
            
            return retVal;
        }

        void CreateObject(JSONType type, string key)
        {
            JSON newObject = null;
            if(type == JSONType.Array)
            {
                newObject = new JSONArray();
            }
            else
            {
                newObject = new JSONObject();
            }
            if(_currentObject != null)
            {
                if(_currentObject.GetJSONType() == JSONType.Array)
                {
                    _currentObject.AsArray().Add(newObject);
                }
                else
                {
                    _currentObject.AsObject().Add(key, newObject);
                }
            }
            _stack.Push(newObject);
            _currentObject = newObject;
            MoveToNextNotEmptyPosition();
        }

        JSONValue ParseValue()
        {
            bool boolValue;
            if(TryParseBool(out boolValue))
            {
                return new JSONBool(boolValue);
            }
            bool isFloat;
            if(IsNumber(out isFloat))
            {
                if(isFloat)
                {
                    return new JSONFloat(ParseFloat());
                }
                long longValue = ParseLong();
                if(longValue < (long)int.MinValue || longValue > (long)int.MaxValue)
                {
                    return new JSONLong(longValue);
                }
                return new JSONInt((int)longValue);
            }
            if(IsString())
            {
                return new JSONString(ParseString());
            }
            if(_currentPosition + kNull.Length < _serialized.Length)
            {
                bool isNull = true;
                for(int i = 0; isNull && i < kNull.Length; ++i)
                {
                    isNull &= _serialized[_currentPosition + i] == kNull[i];
                }
                if(isNull)
                {
                    MoveToNextNotEmptyPosition();
                }
            }
            return new JSONEmpty();
        }

        string ParseString()
        {
            string retVal = string.Empty;
            int nextIndex = _currentPosition;
            bool closed = false;
            int errorTries = 1000000;
            do
            {
                nextIndex = GetNextTokenIndex(kQuoteToken, nextIndex);
                closed = _serialized[nextIndex - 1] != '\\';
                --errorTries;
                if(errorTries < 0)
                {
                    throw new MalformedJSONException("Malformed or too long escaped string");
                }
            }
            while (!closed);
            int length = nextIndex - _currentPosition;
            if(length > 0)
            {
                retVal = _serialized.Substring(_currentPosition + 1, length - 1);
                _currentPosition += length;
                MoveToNextNotEmptyPosition();
            }
            return retVal;
        }

        bool IsBool()
        {
            bool boolValue;
            return TryParseBool(out boolValue);
        }

        bool TryParseBool(out bool value)
        {
            char currentChar = _serialized[_currentPosition];
            if(_currentPosition + kTrue.Length < _serialized.Length && (currentChar == kTrue[0] || currentChar == kTrueCap[0]))
            {
                bool isTrue = true;
                for(int i = 1; isTrue && i < kTrue.Length - 1; ++i)
                {
                    isTrue &= _serialized[_currentPosition + i] == kTrue[i];
                }
                if(isTrue)
                {
                    SetNextToken();
                    value = true;
                    return true;
                }
            }
            if(_currentPosition + kFalse.Length < _serialized.Length && (currentChar == kFalse[0] || currentChar == kFalseCap[0]))
            {
                bool isFalse = true;
                for(int i = 1; isFalse && i < kFalse.Length - 1; ++i)
                {
                    isFalse &= _serialized[_currentPosition + i] == kFalse[i];
                }
                if(isFalse)
                {
                    SetNextToken();
                    value = false;
                    return true;
                }
            }
            value = false;
            return false;
        }

        bool IsString()
        {
            return _currentToken == kQuoteToken;
        }

        bool IsNumber()
        {
            bool isFloat;
            return IsNumber(out isFloat);
        }

        bool IsNumber(out bool isFloat)
        {
            int length = SearchNextNotNumber() - _currentPosition;
            int index = _currentPosition;
            char currentChar = _serialized[index];
            isFloat = false;
            if(currentChar == kPlus || currentChar == kMinus)
            {
                ++index;
            }
            for(; index < _currentPosition + length; ++index)
            {
                currentChar = _serialized[index];
                if((currentChar < kZero || currentChar > kNine) && currentChar != 'e' && currentChar != kMinus && currentChar != kPlus)
                {
                    if(currentChar == kDot && !isFloat)
                    {
                        isFloat = true;
                    }
                    else
                    {
                        isFloat = false;
                        return false;
                    }
                }
            }
            return true;
        }

        long ParseLong()
        {
            long retVal = 0;
            int length = SearchNextNotNumber() - _currentPosition;
            if(length > 0)
            {
                string substring = _serialized.Substring(_currentPosition, length);
                if(!long.TryParse(substring, out retVal))
                {
                    float floatVal = 0.0f;
                    retVal = float.TryParse(substring, NumberStyles.Integer, CultureInfo.InvariantCulture, out floatVal) ? (long)floatVal : 0;
                }
                SetNextToken();
            }
            return retVal;
        }

        float ParseFloat()
        {
            float retVal = 0.0f;
            int length = SearchNextNotNumber() - _currentPosition;
            if(length > 0)
            {
                string substring = _serialized.Substring(_currentPosition, length);
                if(!float.TryParse(substring, NumberStyles.Float, CultureInfo.InvariantCulture, out retVal))
                {
                    retVal = 0.0f;
                }
                SetNextToken();
            }
            return retVal;
        }

        void SetNextToken()
        {
            _currentPosition = SearchNextToken();
            _currentToken = _serialized[_currentPosition];
        }

        int SearchNextOpenToken()
        {
            for(int i = _currentPosition + 1; i < _serialized.Length; ++i)
            {
                var currentChar = _serialized[i];
                if(currentChar == kOpenObjectToken ||
                   currentChar == kOpenArrayToken)
                {
                    return i;
                }
            }
            return -1;
        }

        int SearchNextNotNumber()
        {
            for(int i = _currentPosition + 1; i < _serialized.Length; ++i)
            {
                var currentChar = _serialized[i];
                if((currentChar < kZero || currentChar > kNine) &&
                   !(currentChar == kDot || currentChar == kPlus || currentChar == kMinus || currentChar == 'e'))
                {
                    return i;
                }
            }
            return -1;
        }

        int SearchNextToken()
        {
            if(_prevPosition == _currentPosition)
            {
                return _nextTokenIndex;
            }
            _prevPosition = _currentPosition;
            for(int i = _currentPosition + 1; i < _serialized.Length; ++i)
            {
                var currentChar = _serialized[i];
                if(_objectTokens.Contains(currentChar))
                {
                    _nextTokenIndex = i;
                    return _nextTokenIndex;
                }
            }
            return -1;
        }

        char MoveToNextNotEmptyPosition()
        {
            ++_currentPosition;
            if(_currentPosition >= _serialized.Length)
            {
                throw new MalformedJSONException("Malformed json");
            }
            char currentChar = _serialized[_currentPosition];
            while(_currentPosition < _serialized.Length - 1 &&
                  !_allTokens.Contains(currentChar) &&
                  !IsNumber(currentChar))
            {
                ++_currentPosition;
                currentChar = _serialized[_currentPosition];
            }
            _currentToken = currentChar;
            return currentChar;
        }

        int GetNextTokenIndex(char token, int fromPosition)
        {
            for(int i = fromPosition + 1; i < _serialized.Length; ++i)
            {
                if(_serialized[i] == token)
                {
                    return i;
                }
            }
            return -1;
        }

        static bool IsNumber(char input)
        {
            return !(input < kZero || input > kNine);
        }
    }
}
