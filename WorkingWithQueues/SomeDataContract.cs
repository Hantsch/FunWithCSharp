using System;

namespace WorkingWithQueues
{
    public class SomeDataContract
    {
        /// <summary>
        /// Always use a parameterless constructor
        /// for serialization or entitiy framework
        /// or other fancy shit
        /// </summary>
        public SomeDataContract()
        {
            // we don't want to create a new guid because
            // this constructor should not be called from a human beeing
        }

        /// <summary>
        /// Define a constructor for our friends
        /// to be not confused how to create a propper instance of this object
        /// </summary>
        public SomeDataContract(string message)
        {
            this.SomeUniqueId = Guid.NewGuid().ToString();
            this.AMessageDuh = message;
        }

        public string SomeUniqueId { get; set; }

        public string AMessageDuh { get; set; }
    }
}
