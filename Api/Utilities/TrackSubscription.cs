namespace ASG.ApiService.Utilities
{
    public class TrackSubscription
    {
        public long NumberRequests { get; set; }
        public long CurrentNumberRequests { get; set; }
        public string Status { get; internal set; }
        public bool CancelAtPeriodEnd { get; internal set; }

        public bool IsSubscribe => (Status == Utilities.Status.Active) && !CancelAtPeriodEnd;
        public bool Canceled => (Status == Utilities.Status.Canceled) || CancelAtPeriodEnd;
        public bool IsAllowed => (IsSubscribe && CurrentNumberRequests <= NumberRequests && NumberRequests != 0);
    }
}
