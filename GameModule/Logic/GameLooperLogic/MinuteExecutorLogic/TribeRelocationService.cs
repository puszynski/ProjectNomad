using GameModule.Entities;
using GameModule.Repositories;

namespace GameModule.Logic.GameLooperLogic.MinuteExecutorLogic
{
    internal interface ITribeRelocationService
    {
        internal void ScheduleRelocationProcess();
        internal bool IsValidToStartRelocationProcess();
        internal void StartRelocationProcess();
        internal void EndRelocationProcess();
    }

    internal class TribeRelocationService //: IRelocationService
    {
        readonly ITribeRelocationRepository _tribeRelocationRepository;
        public TribeRelocationService(ITribeRelocationRepository tribeRelocationRepository)
        {
            _tribeRelocationRepository = tribeRelocationRepository;
        }

        internal async void ScheduleRelocationProcess(HumanUnitTaskOrder humanUnitTaskOrder, Tribe tribe)
        {
            var entity = new TribeRelocation
            {
                From = null,
                To = null,

                Start = new Entities.ValueObjects.Localization { X = tribe.Localization.X, Y = tribe.Localization.Y },
                Destiny = new Entities.ValueObjects.Localization { X = task}
            };
            await _tribeRelocationRepository.AddAsync(entity);
        }
    }
}
