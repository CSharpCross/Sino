namespace OrvilleX.Application.Dto
{
    public interface IEntityDto<TPrimaryKey> : IDto
	{
		/// <summary>
		/// 主键
		/// </summary>
		TPrimaryKey Id { get; set; }
	}
}