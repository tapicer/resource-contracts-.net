using System;
using Contracts;

namespace PersonExample
{
    public class Person
    {
        public string FirtName { get; private set; }
        public string LastName { get; private set; }
        public Address Address { get; private set; }

        public void MoveTo(string street, string city, string state)
        {
            Contract.Memory.Rsd<Address>(Contract.Memory.This, 1);
            Contract.Memory.Tmp<AddressValidator>(1);

            Contract.Memory.DestRsd(Contract.Memory.This);
            this.Address = new Address(street, city, state);
        }

        public Person(string firstName, string lastName, string street, string city, string state)
        {
            Contract.Memory.Rsd<Address>(Contract.Memory.This, 1);
            Contract.Memory.Tmp<AddressValidator>(1);

            this.FirtName = firstName;
            this.LastName = lastName;

            Contract.Memory.AddRsd(Contract.Memory.This, Contract.Memory.This);
            this.MoveTo(street, city, state);
        }
    }
}
