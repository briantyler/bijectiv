namespace Bijectiv.Builder
{
    using System.Collections;
    using System.Collections.Generic;

    public class TransformDefinitionRegistry : ITransformDefinitionRegistry
    {
        private readonly List<TransformDefinition> defintions = new List<TransformDefinition>();

        public IEnumerator<TransformDefinition> GetEnumerator()
        {
            return this.defintions.GetEnumerator();
        }

        public void Add(TransformDefinition definition)
        {
            this.defintions.Add(definition);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.defintions).GetEnumerator();
        }
    }
}