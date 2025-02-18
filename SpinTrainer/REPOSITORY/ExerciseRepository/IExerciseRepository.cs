using ENTITYS;

namespace REPOSITORY.ExerciseRepository
{
    public interface IExerciseRepository
    {
        /// <summary>
        /// Obtiene la información de un ejercicio por su ID.
        /// </summary>
        /// <param name="id">ID del ejercicio.</param>
        /// <returns>Devuelve el modelo de datos del ejercicio, un bool para indicar si la operación fue exitosa y un mensaje de error en caso de que lo de.</returns>
        Task<(ExerciseEntity,bool,string)> GetById(int id);

        /// <summary>
        /// Obtiene una lista genérica de los ejercicios almacenados en la base de datos.
        /// </summary>
        /// <returns>Devuelve una lista genérica (IEnumerable) con los datos de los ejercicios, un bool para indicar si la operación fue exitosa y un mensaje de error en caso de que lo de.</returns>
        Task<(IEnumerable<ExerciseEntity>,bool,string)> GetAll();
    }
}
