namespace Contracts.Appointment.AppointmentHelpers;

public class GetAllAppointmentsRequest
{
    public int? OrganizationId { get; }
    
    public int? UserId { get; }

    public GetAllAppointmentsRequest(int? organizationId, int? userId)
    {
        OrganizationId = organizationId;
        UserId = userId;
    }
    
    public static GetAllAppointmentsRequestBuilder Builder => new ();

    public class GetAllAppointmentsRequestBuilder
    {
        private int? organizationId;
        private int? userId;

        public GetAllAppointmentsRequestBuilder WithOrganizationId(int? organizationId)
        {
            this.organizationId = organizationId;
            return this;
        }

        public GetAllAppointmentsRequestBuilder WithUserId(int? userId)
        {
            this.userId = userId;
            return this;
        }

        public GetAllAppointmentsRequest Build()
        {
            return new GetAllAppointmentsRequest(this.organizationId, this.userId);
        }
    }
}