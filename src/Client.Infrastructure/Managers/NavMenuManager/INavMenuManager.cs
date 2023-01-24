using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.NavMenuManager
{
	public interface INavMenuManager:IManager
	{
		public Task<IDictionary<string, MenuItem>> GetNavAndPinnedMenus();

		Task<BLUIElement> GetMenuUIElement(ObjectFormRequest request);

		Task UpdatePinnedMenus(MenuItem menurequest);
		Task<UserConfigObjectsBlLite> LoadObjectsForUserConfiguration(ObjectFormRequest request);
		Task UpdateObjectsForUserConfiguration(UserConfigObjectsBlLite request);
	}
}
