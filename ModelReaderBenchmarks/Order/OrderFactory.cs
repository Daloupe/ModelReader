namespace ModelReader.Sorters
{
    public static class OrderFactory
    {
        public static IOrderVisitor Create((string property, string direction) sorter)
        {
            return Create(sorter.property, sorter.direction);
        }

        public static IOrderVisitor Create(string property, string direction)
        {
            switch (direction.ToUpper())
            {
                case "DESC":
                    return DescendingVisitor.Create(property);

                default:
                    return AscendingVisitor.Create(property);
            }
        }
    }
}
