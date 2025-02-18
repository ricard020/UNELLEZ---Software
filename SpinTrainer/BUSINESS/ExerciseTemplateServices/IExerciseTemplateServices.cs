using ENTITYS;

namespace SERVICES.ExerciseTemplateServices
{
    public interface IExerciseTemplateServices
    {
        /// <summary>
        /// Guarda el ejercicio de descanso.
        /// </summary>
        /// <returns>Un bool para saber si la operación fue exitosa y un string para mensaje de error en caso de que lo de.</returns>
        Task<(bool, string)> SaveRestingExercise(ExerciseTemplateEntity newExercise);

        /// <summary>
        /// Obtiene el ejercicio de descanso de la base de datos.
        /// </summary>
        /// <returns>El modelo de datos de ejercicio, un bool para saber si la operación fue exitosa y un string para mensaje de error en caso de que lo de.</returns>
        Task<(ExerciseTemplateEntity, bool, string)> GetRestingExercise();
    }
}
