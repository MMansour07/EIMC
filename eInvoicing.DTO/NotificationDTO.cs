using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public enum NotificationType
    {
        DelegationInvite = 1,
        OTP = 2,
        ProfileDataValidation = 3,
        GenericNotification = 4,
        ReceiveDownloadReady = 5,
        DocumentReceived = 6,
        DocumentValidated = 7,
        DocumentCancelled = 8,
        DocumentRejected = 9,
    }
    public class NotificationDTO
    {
        public int notificationId { get; set; }
        public string receiverName { get; set; }
        public string receiverType { get; set; }
        public string creationDateTime { get; set; }
        public string deliveredDateTime { get; set; }
        public string receivedDateTime { get; set; }
        public string messageData { get; set; }
        public NotificationType typeId { get; set; }
        public string status { get; set; }
        public string typeName { get; set; }
        public string notificationDeliveryId { get; set; }
        public string finalMessage { get; set; }
        public string address { get; set; }
        public string channel { get; set; }

    }
}
