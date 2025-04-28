namespace WriteFacade.Crayon.DTO
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public int SoftwareId { get; set; }
        public string SoftwareName { get; set; }
        public string SoftwareDescription { get; set; }
        public bool Active { get; set; }
        public DateTime ValidUntil { get; set; }
        public virtual List<LicenseDto> Licenses { get; set; } = new List<LicenseDto>();
    }
}
