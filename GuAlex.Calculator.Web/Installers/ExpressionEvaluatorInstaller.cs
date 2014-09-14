namespace GuAlex.Calculator.Web.Installers
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class ExpressionEvaluatorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IExpressionEvaluator>().ImplementedBy<SimpleExpressionEvaluator>());
        }
    }
}