namespace Contracts.Appointment.SlotHelpers;

public class GetAllSlotsRequest
{
    public int? OrganizationId { get; }
    public int? PlaceId { get; }
    public bool? IsFree { get; }
    public DateTime From { get; }
    public DateTime To { get; }

    internal GetAllSlotsRequest(int? organizationId, int? placeId, bool? isFree, DateTime? from, DateTime? to)
    {
        OrganizationId = organizationId;
        PlaceId = placeId;
        IsFree = isFree;
        if (from == null)
        {
            From = DateTime.MinValue;
        }
        else
        {
            From = from.Value;
        }

        if (to == null)
        {
            To = DateTime.MaxValue;
        }
    }
    
    public static GetAllSlotsRequestBuilder Builder => new GetAllSlotsRequestBuilder();

    public class GetAllSlotsRequestBuilder
    {
        private int? organizationId;
        private int? placeId;
        private bool? isFree;
        private DateTime from;
        private DateTime to;

        public GetAllSlotsRequestBuilder WithOrganizationId(int organizationId)
        {
            this.organizationId = organizationId;
            return this;
        }

        public GetAllSlotsRequestBuilder WithPlaceId(int placeId)
        {
            this.placeId = placeId;
            return this;
        }

        public GetAllSlotsRequestBuilder WithIsFree(bool isFree)
        {
            this.isFree = isFree;
            return this;
        }

        public GetAllSlotsRequestBuilder WithFrom(DateTime from)
        {
            this.from = from;
            return this;
        }

        public GetAllSlotsRequestBuilder WithTo(DateTime to)
        {
            this.to = to;
            return this;
        }

        public GetAllSlotsRequest Build()
        {
            return new GetAllSlotsRequest(organizationId, placeId, isFree, from, to);
        }
    }
}