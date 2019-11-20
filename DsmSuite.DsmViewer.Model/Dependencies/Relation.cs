﻿namespace DsmSuite.DsmViewer.Model.Dependencies
{
    /// <summary>
    /// Relation between two elements.
    /// </summary>
    public class Relation
    {
        private readonly char _typeId;
        private static readonly TypeRegistration TypeRegistration = new TypeRegistration();

        public Relation(int consumerId, int providerId, string type, int weight)
        {
            ConsumerId = consumerId;
            ProviderId = providerId;
            _typeId = TypeRegistration.AddTypeName(type);
            Weight = weight;
        }

        /// <summary>
        /// Unique id of consumer element.
        /// </summary>
        public int ConsumerId { get; private set; }

        /// <summary>
        /// Unique id of provider element.
        /// </summary>
        public int ProviderId { get; private set; }

        /// <summary>
        /// Type of relation.
        /// </summary>
        public string Type => TypeRegistration.GetTypeName(_typeId);

        /// <summary>
        /// Strength or weight of the relation
        /// </summary>
        public int Weight { get; private set; }
    }
}
