namespace SHARKNA.Models
{
    public class tblEventAttendence
    {
        public Guid Id { get; set; }
        public DateTime EventDate { get; set; }

        public int Day { get; set; }

        public tblEvents Events { get; set; }
        public Guid EventsId { get; set; }


        public tblEventRegistrations EventsReg { get; set; }
        public Guid EventsRegId { get; set; }
        public bool IsAttend { get; set; }





    }
}