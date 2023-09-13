using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace CarBoyWebservice.MBProto.Exceptions
{

    public class AuthException : Exception
    {
        ErrorType _ErrorTypeProperties;

        public AuthException(ErrorType type)
        {
            _ErrorTypeProperties = type;
        }

        public T Serialize<T>()
        {

            if (typeof(T) == typeof(HttpStatusCode))
                return (T)Convert.ChangeType(_ErrorTypeProperties.Code, typeof(T));
            else if (typeof(T) == typeof(string))
                return (T)Convert.ChangeType(_ErrorTypeProperties.Message, typeof(T));

            throw new InvalidCastException("HttpStatusCode & string are valid types");
        }
        #region push
        public class TokenInvalid : ErrorType
        {
            public TokenInvalid() : base("TOKEN_INVALID", HttpStatusCode.BadRequest) { }
        }
        public class DeviceIdInvalid : ErrorType
        {
            public DeviceIdInvalid() : base("DEVICE_ID_INVALID", HttpStatusCode.BadRequest) { }
        }
        #endregion
        #region public
        public class KilometerInvalid : ErrorType
        {
            public KilometerInvalid() : base("KILOMETER_INVALID", HttpStatusCode.BadRequest) { }
        }
        public class PageIndexInvalid : ErrorType
        {
            public PageIndexInvalid() : base("PAGE_INDEX_INVALID", HttpStatusCode.BadRequest) { }
        }
        public class PageCountInvalid : ErrorType
        {
            public PageCountInvalid() : base("PAGE_COUNT_INVALID", HttpStatusCode.BadRequest) { }
        }
        #endregion

        #region dist
        public class AccessDenied : ErrorType
        {
            public AccessDenied() : base("ACCESS_DENIED", HttpStatusCode.NotAcceptable) { }
        }
        public class DateTimeInvalid : ErrorType
        {
            public DateTimeInvalid() : base("DATE_TIME_INVALID", HttpStatusCode.BadRequest) { }
        }
        public class LocationNotFound : ErrorType
        {
            public LocationNotFound() : base("LOCATION_NOT_FOUND", HttpStatusCode.NotFound) { }
        }

        #endregion

        #region product

        public class ProductListInvalid : ErrorType
        {
            public ProductListInvalid() : base("PRODUCT_LIST_INVALID", HttpStatusCode.BadRequest) { }
        }

        #endregion

        #region user
        public class PasswordInvalid : ErrorType
        {
            public PasswordInvalid() : base("PASSWORD_INVALID", HttpStatusCode.BadRequest) { }
        }
        public class UserNotRegistered : ErrorType
        {
            public UserNotRegistered() : base("USER_NOT_REGISTERED", HttpStatusCode.BadRequest) { }
        }
        public class PhoneNumberInvalid : ErrorType
        {
            public PhoneNumberInvalid() : base("PHONE_NUMBER_INVALID", HttpStatusCode.BadRequest) { }
        }
        public class ProductHashInvalid : ErrorType
        {
            public ProductHashInvalid() : base("PRODUCT_HASH_INVALID", HttpStatusCode.BadRequest) { }
        }

        public class SmsCodeInvalid : ErrorType
        {
            public SmsCodeInvalid() : base("SMS_CODE_INVALID", HttpStatusCode.BadRequest) { }
        }
        public class SmsCodeExpired : ErrorType
        {
            public SmsCodeExpired() : base("SMS_CODE_EXPIRED", HttpStatusCode.BadRequest) { }
        }
        #endregion

        #region service

        public class ServiceOrderHashInvalid : ErrorType
        {
            public ServiceOrderHashInvalid() : base("SERVICE_ORDER_HASH_INVALID", HttpStatusCode.BadRequest) { }
        }

        public class ServiceInvalid : ErrorType
        {
            public ServiceInvalid() : base("SERVICE_INVALID", HttpStatusCode.BadRequest) { }
        }

        public class ServiceCancelByCustomer : ErrorType
        {
            public ServiceCancelByCustomer() : base("SERVICE_CANCEL_BY_CUSTOMER", HttpStatusCode.BadRequest) { }
        }

        public class ServiceCancelByOperator : ErrorType
        {
            public ServiceCancelByOperator() : base("SERVICE_CANCEL_BY_OPERATOR", HttpStatusCode.BadRequest) { }
        }
        public class ServiceProductNotDelivered : ErrorType
        {
            public ServiceProductNotDelivered() : base("SERVICE_PRODUCT_NOT_DELIVERED", HttpStatusCode.BadRequest) { }
        }
        #endregion

        #region payment
        public class PaymentInvalid : ErrorType
        {
            public PaymentInvalid() : base("PAYMENT_INVALID", HttpStatusCode.BadRequest) { }
        }
        #endregion

        public abstract class ErrorType
        {
            string message = "";
            HttpStatusCode code;
            public HttpStatusCode Code { get { return code; } }
            public string Message { get { return message; } }

            public ErrorType(string message, HttpStatusCode code)
            {
                this.message = message;
                this.code = code;
            }
        }
    }

}