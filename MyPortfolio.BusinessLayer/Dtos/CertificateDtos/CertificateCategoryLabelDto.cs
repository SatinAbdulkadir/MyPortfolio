namespace MyPortfolio.BusinessLayer.Dtos.CertificateDtos
{
    // Sertifika kartında gösterilen kategori etiketi
    public class CertificateCategoryLabelDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Icon { get; set; }
        public int? ParentId { get; set; }
        public string? ParentName { get; set; }

        // "Yazılım › Front End"; ana kategoride sadece "Yazılım"
        public string FullName => ParentName == null ? Name : $"{ParentName} › {Name}";
    }
}
