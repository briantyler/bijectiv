namespace Bijectiv.TestUtilities.TestTypes
{
    public class AutoTransformTestClass1
    {
        public int FieldInt;

        public BaseTestClass1 FieldBase;

        public int PropertyInt { get; set; }

        public BaseTestClass1 PropertyBase { get; set; }

        public SealedClass1 PropertySealed { get; set; }
    }
}