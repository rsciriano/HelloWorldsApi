using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Primitives;

public static class HttpResponseMessageAssertionsExtensions
{
    public static AndConstraint<TAssertions> HaveJsonContentEquivalentTo<TAssertions, TContent>(
        this HttpResponseMessageAssertions<TAssertions> assertions,
        TContent expectedContent,
        Func<EquivalencyAssertionOptions<TContent>, EquivalencyAssertionOptions<TContent>> config = null,
        string because = "",
        params object[] becauseArgs
    )
        where TAssertions : HttpResponseMessageAssertions<TAssertions>
    {
        var success = Execute.Assertion
            .ForCondition(assertions.Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected HttpContent to be of type {0} {{reason}}, but HttpResponseMessage was <null>.", typeof(TContent).Name);

        TContent actualContent = default;
        if (success)
        {
            Exception contentException = null;
            try
            {
                actualContent = assertions.Subject.Content.ReadFromJsonAsync<TContent>().Result;
            }
            catch (Exception ex)
            {
                contentException = ex;
            }

            success = Execute.Assertion
                .ForCondition(contentException is null)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected HttpContent to be of type {0} {reason}, but an exception has been throw. Exception message: {1} Response content: {2}.",
                    typeof(TContent).Name,
                    contentException?.Message,
                    assertions.Subject.Content.ReadAsStringAsync().Result);
        }

        if (success)
        {
            if (config is null)
            {
                actualContent.Should().BeEquivalentTo(expectedContent, because, becauseArgs);
            }
            else
            {
                actualContent.Should().BeEquivalentTo(expectedContent, config, because, becauseArgs);
            }
        }
        

        return new AndConstraint<TAssertions>((TAssertions) assertions);
    }
}
