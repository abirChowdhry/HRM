using HRM.DTOs;
using HRM.Migrations;

namespace HRM.Interfaces
{
    public interface IBonusService
    {
        #region=================== Bonus Setup ===================================
        public Task<bool> CreateBonusSetup(BonusSetupCreateVM bonusSetupCreateVM);
        public Task<bool> UpdateBonusSetup(BonusSetupCreateVM bonusSetupCreateVM);
        public Task<List<BonusLanding>> BonusLanding(long intBusinessUnitId);
        #endregion=================== Bonus Setup ===================================

        #region====================== Bonus Generate ==================================
        public Task<bool> CreateBonusGenerate(BonusGenerateCreate bonusGenerateCreate);
        public Task<bool> UpdateBonusGenerate(BonusGenerateCreate bonusGenerateCreate);
        public Task<List<BonusGenerateLanding>> BonusGenerateLanding(long intBusinessUnitid, long intyearId, long intMonthId);
        #endregion====================== Bonus Generate ==================================

    }
}
