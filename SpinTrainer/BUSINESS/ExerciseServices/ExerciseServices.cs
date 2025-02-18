using ENTITYS;
using REPOSITORY.ExerciseRepository;
using System.Collections.ObjectModel;

namespace SERVICES.ExerciseServices
{
    public class ExerciseServices : IExerciseServices
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseServices(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task<(ObservableCollection<ExerciseEntity>, bool, string)> GetAll()
        {
            var (exercisesCollection, operationComplete, errorMessage) = await _exerciseRepository.GetAll();

            var exercises = new ObservableCollection<ExerciseEntity>(exercisesCollection);

            return (exercises, operationComplete, errorMessage);
        }

        public async Task<(ExerciseEntity, bool, string)> GetById(int id)
        {
            return await _exerciseRepository.GetById(id);
        }
    }
}
