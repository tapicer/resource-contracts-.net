using System;
using Contracts;

namespace PersonExample
{
    public class Address
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        
        public Address(string street, string city, string state)
        {
            Contract.Memory.Tmp<AddressValidator>(1);

            Contract.Memory.DestTmp();
            AddressValidator validator = new AddressValidator();

            if (!validator.IsAddressValid(street, city, state))
            {
                throw new Exception("Address is invalid.");
            }

            this.Street = street;
            this.City = city;
            this.State = state;
        }
    }
}
