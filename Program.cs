using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace LinqLesson
{
    class Program
    {
        static void Main(string[] args)
        {
            var persons = JsonConvert.DeserializeObject<IEnumerable<Person>>(File.ReadAllText("data.json"));

            //Task1 
            var northernmost = persons.Max(p => p.Latitude + " " + p.Name);
            var southernmost = persons.Min(p => p.Latitude + " " + p.Name);
            var easternmost = persons.Max(p => p.Longitude + " " + p.Name);
            var westernmost = persons.Min(p => p.Longitude + " " + p.Name);
            Console.WriteLine("The northernmost one is:" + northernmost);
            Console.WriteLine("The southernmost one is:" + southernmost);
            Console.WriteLine("The easternmost one is:" + easternmost);
            Console.WriteLine("The westernmost one is:" + westernmost);

            //Task2
            var distanceMax = persons.SelectMany
                (p1 => persons.Select
                (p2 => new
                {
                    Person1 = p1,
                    Person2 = p2,
                    distance = GetDistance(p1, p2)
                }
                )
                )
                .Where(g => g.Person1 != g.Person2)
                .Max(g => g.distance);
            Console.WriteLine("The max distance between 2 persons is:" + distanceMax);

            var distanceMin = persons.SelectMany
                (p1 => persons.Select
                (p2 => new
                {
                    Person1 = p1,
                    Person2 = p2,
                    distance = GetDistance(p1, p2)
                }
                )
                )
                .Where(g => g.Person1 != g.Person2)
                .Min(g => g.distance);
            Console.WriteLine("The min distance between 2 persons is:" + distanceMin);

            //Task3
            var coupleWithSameWords = persons.SelectMany
                (p3 => persons.Select
                (p4 => new
                {
                    Person3 = p3,
                    Person4 = p4,
                    Same = p3.About.Split(' ').Intersect(p4.About.Split(' ')).Count()
                }))
                            .Where(g => g.Person3 != g.Person4)
                            .Max(g => g.Same);

            //Task4
            var friends = persons
                .SelectMany(person => person.Friends, (person, friend) => new { FriendName = friend.Name, PersonName = person.Name })
                .GroupBy(f => f.FriendName)
                .Where(f => f.Count() > 1)
                .ToList();
            foreach (var q in friends)
            {
                Console.WriteLine($"{q} is common friend for:");
                foreach (var person in q)
                {
                    Console.WriteLine(person.PersonName);
                }
                Console.WriteLine();
            }
        }

        static double GetDistance(Person Person1, Person Person2)
        {
            var geoDiff = Math.Acos((Math.Sin(Person1.Latitude) * Math.Sin(Person2.Latitude)) + Math.Cos(Person1.Latitude) * Math.Cos(Person2.Latitude) * Math.Cos(Person2.Longitude - Person1.Longitude));
            return geoDiff;
        }
    }


}

//checked
