using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ModelReader
{
    public sealed class TestClass : IEquatable<TestClass>
    {
        static Random r = new Random();

        public TestClass()
        {
            Name = Environment.TickCount.ToString();
            Number = r.Next();
            Bool = r.Next(0, 1) == 1;
        }

        public static FilterDefinitionBuilder<TestClass> Filter => Builders<TestClass>.Filter;
        
        public bool Bool { get; set; }

        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }

        public bool Equals(TestClass other)
        {
            return Id == other.Id
                && Name == other.Name
                && Number == other.Number
                && Bool == other.Bool;
        }

        public override bool Equals(object obj)
        {
            return (obj is TestClass other) && Equals(other);
        }

        public override int GetHashCode()
        {
            return Bool.GetHashCode() ^ Number ^ Id.GetHashCode() ^ Name.GetHashCode();
        }
    }
}
