using dropkick.Configuration.Dsl;
using dropkick.Configuration.Dsl.RoundhousE;

namespace BabelTrout.DatabaseDeploy
{
    public class DatabaseDeployment : Deployment<DatabaseDeployment, DeploymentSettings>
    {
        #region Constructors

        public DatabaseDeployment()
        {
            Define(settings => DeploymentStepsFor(Db, s => s.RoundhousE()
              .ForEnvironment(settings.Environment)
              .OnDatabase(settings.DbName)
              .WithScriptsFolder(settings.DbSqlFilesPath)
              .WithDatabaseRecoveryMode(settings.DbRecoveryMode)
              .WithRestorePath(settings.DbRestorePath)
              .WithRepositoryPath("__REPLACE_ME__")
              .WithVersionFile("_BuildInfo.xml")
              .WithRoundhousEMode(settings.RoundhousEMode)));
        }

        #endregion

        #region Properties

        //order is important
        public static Role Db { get; set; }

        #endregion
    }
}