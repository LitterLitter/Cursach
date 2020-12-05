using System;

namespace Validation.Forms
{
    /// <summary>
    /// Validation rule for form
    /// </summary>
    class ValidationRule
    {

        // if set to true - this rule will alway return false.
        // this flag is set if during rule checking exception is occured.
        bool wasException = false;

        /// <summary>
        /// Condition that must return true if validation is successfull
        /// </summary>
        public Func<bool> Condition { get; private set; }

        /// <summary>
        /// Validation type
        /// </summary>
        public ValidationType ValidationType { get; private set; }

        /// <summary>
        /// Message to display for user if condition fails
        /// </summary>
        public string Message { get; private set; }

        public ValidationRule(Func<bool> condition, string message,
            ValidationType validationType = ValidationType.Required)
        {
            Condition = condition;
            Message = message;
            ValidationType = validationType;
        }

        /// <summary>
        /// Check rule and return true if validated successfully
        /// </summary>
        public bool Check()
        {
            if (!wasException)
            {
                try
                {
                    bool result = Condition();
                    return result;
                }
                catch (Exception ex)
                {
                    wasException = true;
                }
            }
            return false;
        }
    }
}
