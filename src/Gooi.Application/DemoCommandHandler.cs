
using MediatR;

namespace Skinder.Gooi.Application;


 public class DemoCommandHandler : IRequestHandler<DemoCommand, int>
  {

    // public DemoCommandHandler(IOutputWriter outputWriter, IAttributeRepository attributesRepository)
    // {
    //   _outputWriter = outputWriter;
    //   _attributesRepository = attributesRepository;
    // }

    public async Task<int> Handle(DemoCommand request, CancellationToken cancellationToken)
    {
    // _outputWriter.WriteHeading("The Chef is adding your attributes");

    // Dictionary<string, string> attributes = DatabaseAttributeParser.Parse(request.Attributes);

    // //check for existing attributes
    // IEnumerable<string> existingAttributeNames = await _attributesRepository.GetAttributeNames();
    // IEnumerable<string> duplicateAttributes = attributes.Keys.Where(k => existingAttributeNames.Any(i => i.Equals(k, StringComparison.InvariantCultureIgnoreCase)));
    // foreach (string duplicateAttribute in duplicateAttributes)
    // {
    //   attributes.Remove(duplicateAttribute);
    //   _outputWriter.WriteWarning($"Database attribute '{duplicateAttribute}' already exists on the target database. Skipping...");
    // }

    // int result = await _attributesRepository.AddAttributes(attributes);

    // if (result > 0)
    // {
    //   _outputWriter.WriteSuccess($"Added {result} database attribute(s) successfully!");
    // }
    // else
    // {
    //   _outputWriter.WriteLine("There were no database attributes added.");
    // }

    Console.WriteLine("Woop woop yay!");
    return 0;
    }
  }