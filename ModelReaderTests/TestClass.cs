using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ModelReader
{
    sealed public class TestClass : IEquatable<TestClass>
    {
        static Random r = new Random();

        public TestClass()
        {
            Name = Environment.TickCount.ToString();
            Number = r.Next();
            Bool = r.Next(0, 1) == 1;
        }

        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public bool Bool { get; set; }

        public bool Equals(TestClass other)
        {
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Number;
        }
    }
}
