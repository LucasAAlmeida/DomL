﻿using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.Services
{
    public class PersonService
    {
        public static Person GetOrCreateByName(string personName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(personName)) {
                return null;
            }

            Person person = unitOfWork.PersonRepo.SingleOrDefault(u => Util.IsEqualString(u.Name, personName));

            if (person == null) {
                person = new Person() {
                    Name = personName
                };
                unitOfWork.PersonRepo.Add(person);
            }

            return person;
        }
    }
}