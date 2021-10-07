using FluentValidation;

using MLDB.Api.DTO;

public class SurveyDTOValidator : AbstractValidator<SurveyDTO> {
  public SurveyDTOValidator() {
    RuleFor(survey => survey.SurveyDate).NotNull();
  }
}