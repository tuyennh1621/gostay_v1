using Google.Cloud.Firestore;

namespace GoStay.Web.Areas.Admin.Model
{
    [FirestoreData]
    public class TestFirebase
    {
        public string TestFirebaseId { get; set; }
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]

        public string Name { get; set; }
        [FirestoreProperty]
        public string Mess { get; set; }
    }
}
