using GoStay.DataAccess.Entities;

namespace GoStay.DataDto.Apis
{
	public class ResponseBase<T>
	{
		public int Code { get; set; }
		public int Count { get; set; }
		public string Message { get; set; }
		public bool IsSuccessful { get; set; }
		public T Data { get; set; }
	}
}
