using NUnit.Framework;
using FluentValidation;
using FluentValidation.TestHelper;
using MLDB.Api.DTO;


namespace MLDB.Api.Tests.ValidationTests
{
  [TestFixture]
  public class SurveyDTOValidationTests {
      private SurveyDTOValidator validator;

      [SetUp]
      public void Setup() {
        validator = new SurveyDTOValidator();
      }

      [Test]
      public void Should_FailWhen_DateIsNull() {
        var testSurvey = new SurveyDTO { SurveyDate = null };
        
        var result = validator.TestValidate(testSurvey);
        
        result.ShouldHaveValidationErrorFor(testSurvey => testSurvey.SurveyDate);
      }
  }
}