using System;

namespace PersonExample
{
    public class AddressValidator
    {
        public bool IsAddressValid(string street, string city, string state)
        {
            return !(String.IsNullOrEmpty(street) ||
                     String.IsNullOrEmpty(city) ||
                     String.IsNullOrEmpty(state));
        }
    }
}
