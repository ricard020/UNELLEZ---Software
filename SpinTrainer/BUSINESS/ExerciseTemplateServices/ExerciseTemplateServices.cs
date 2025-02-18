using ENTITYS;
using REPOSITORY.ExerciseTemplateRepository;

namespace SERVICES.ExerciseTemplateServices
{
    public class ExerciseTemplateServices : IExerciseTemplateServices
    {
        private readonly IExerciseTemplateRepository _exerciseTemplateRepository;

        public ExerciseTemplateServices(IExerciseTemplateRepository exerciseTemplateRepository)
        {
            _exerciseTemplateRepository = exerciseTemplateRepository;
        }

        public async Task<(ExerciseTemplateEntity, bool, string)> GetRestingExercise()
        {
            return await _exerciseTemplateRepository.GetRestingExercise();
        }

        public async Task<(bool, string)> SaveRestingExercise(ExerciseTemplateEntity newExercise)
        {
            return await _exerciseTemplateRepository.SaveRestingExercise(newExercise);
        }
    }
}
