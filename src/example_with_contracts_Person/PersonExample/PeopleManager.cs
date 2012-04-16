using System;
using System.Collections.Generic;
using Contracts;

namespace PersonExample
{
    public class PeopleManager
    {
        public static Person CreatePerson(string firstName, string lastName, string street, string city, string state)
        {
            Contract.Memory.Rsd<Person>(Contract.Memory.Return, 1);
            Contract.Memory.Rsd<Address>(Contract.Memory.Return, 1);
            Contract.Memory.Tmp<AddressValidator>(1);

            Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.This);
            Contract.Memory.DestRsd(Contract.Memory.Return);
            Person person = new Person(firstName, lastName, street, city, state);

            return person;
        }

        public static Contract.Memory.RsdType rsd_MovePersonTo_person;

        public static void MovePersonTo(Person person, string street, string city, string state)
        {
            Contract.Memory.BindRsd(rsd_MovePersonTo_person, person);
            Contract.Memory.Rsd<Address>(rsd_MovePersonTo_person, 1);
            Contract.Memory.Tmp<AddressValidator>(1);

            Contract.Memory.AddRsd(rsd_MovePersonTo_person, Contract.Memory.This);
            person.MoveTo(street, city, state);
        }

        public static Person[] CreateFamily(List<string> firstNames, string lastName, string street, string city, string state)
        {
            System.Diagnostics.Contracts.Contract.Requires(firstNames.Count > 0);

            Contract.Memory.Rsd<Person>(Contract.Memory.Return, firstNames.Count);
            Contract.Memory.Rsd<Address>(Contract.Memory.Return, firstNames.Count);
            Contract.Memory.Tmp<AddressValidator>(1);

            Person[] family = new Person[firstNames.Count];

            for (int i = 0; i < firstNames.Count; i++)
            {
                Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.This);
                Contract.Memory.DestRsd(Contract.Memory.Return);
                Person p = new Person(firstNames[i], lastName, street, city, state);
                family[i] = p;
            }

            return family;
        }

        /// <summary>
        /// Creates a family with the combination of all first and last names
        /// </summary>
        public static Person[] CreateCombinedFamily(List<string> firstNames, List<string> lastNames, string street, string city, string state)
        {
            System.Diagnostics.Contracts.Contract.Requires(firstNames.Count > 0);
            System.Diagnostics.Contracts.Contract.Requires(firstNames.Count == lastNames.Count);

            Contract.Memory.Rsd<Person>(Contract.Memory.Return, firstNames.Count * lastNames.Count);
            Contract.Memory.Rsd<Address>(Contract.Memory.Return, firstNames.Count * lastNames.Count);
            Contract.Memory.Tmp<AddressValidator>(1);

            Person[] family = new Person[firstNames.Count * lastNames.Count];

            for (int i = 0; i < firstNames.Count; i++)
            {
                Contract.Memory.IterationSpace(0 <= i && i < firstNames.Count);
                for (int j = 0; j < lastNames.Count; j++)
                {
                    Contract.Memory.IterationSpace(0 <= j && j < lastNames.Count);
                    Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.This);
                    Contract.Memory.DestRsd(Contract.Memory.Return);
                    Person p = new Person(firstNames[i], lastNames[j], street, city, state);
                    family[i * firstNames.Count + j] = p;
                }
            }

            return family;
        }
    }
}
