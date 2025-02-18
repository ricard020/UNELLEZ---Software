namespace ENTITYS.DTOs
{
    public class HandsPositionsDto
    {
        public string Number { get; set; }

        public string RouteImage
        {
            get => $"hp{Number}.jpeg";
        }
    }
}
