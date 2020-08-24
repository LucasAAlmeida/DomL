﻿using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace DomL.Business.Services
{
    public class PersonService
    {
        public static Person GetOrCreateByName(string personName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(personName)) {
                return null;
            }

            var person = GetByName(personName, unitOfWork);

            if (person == null) {
                person = new Person() {
                    Name = personName
                };
                unitOfWork.PersonRepo.Add(person);
            }

            return person;
        }

        public static List<Person> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.PersonRepo.GetAll().ToList();
        }

        public static Person GetByName(string personName, UnitOfWork unitOfWork)
        {
            return unitOfWork.PersonRepo.GetByName(personName);
        }
    }
}