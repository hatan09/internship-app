#addin nuget:?package=Cake.FileHelpers&version=6.0.0

//root folder name - needs to be changed
const string root = "InternshipApp";

//namespace - needs to be changed
string name_space = "InternshipApp";

//folder
var target_Controllers = Argument("target", "Controllers");
var target_IRepo = Argument("target", "");
var target_Repo = Argument("target", "");
var target_Dto = Argument("target", "DataObjects");

//project
var group_Controllers = Argument("group", ".Api");
var group_IRepo = Argument("group", ".Contract");
var group_Repo = Argument("group", ".Repository");
var group_Dto = Argument("group", ".Api");

//entity name - needs to be changed
var name = Argument("name", "Message");

//?
var cardDetail = Argument("cardDetail", "CardDetail");
var originalDocumentUrl = Argument("originalDocumentUrl", "DocumentLink");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

const string FODLER_PATH_TEMPLATE = $"./{root}" + "{0}/{1}";

Task("SetupEntity")
    .Does(() =>
    {
        // each step creates 1 file with content

        #region [ 1. DTO ]
        // - setup path
        var targetFolderPath = string.Format(FODLER_PATH_TEMPLATE, group_Dto, target_Dto);
        Information("* Target folder full path: " + targetFolderPath);

        if (!DirectoryExists(targetFolderPath))
        {   // if folder does not exist => create
            Warning("\nFolder for DTO does not exist. \n>> Creating...");
            //target folder
            CreateDirectory(targetFolderPath);
            Information("\n>> Done!");
        }

        // - generate file
        Information($"\n>> Generate >> {name}DTO.cs");  // log
        if (FileExists($"{targetFolderPath}/{name}DTO.cs"))
        {   // check if file exists
            Warning($"\nError: {name}DTO exists.");
        }
        else FileWriteText(  // fucntion: generate file + content
                        // parameters
                        // 1 - path/filename.extension
            $"{targetFolderPath}/{name}DTO.cs",

            // 2 - content
            $@"namespace {name_space}.DTOs
{{
    public class {name}DTO : BaseDTO
    {{
        
    }}
}}" // end content
        ); // end step
        #endregion

        #region [ 2. IRepo ]
        // - setup path
        targetFolderPath = string.Format(FODLER_PATH_TEMPLATE, group_IRepo, target_IRepo);
        Information("\n* Target folder full path: " + targetFolderPath);

        if (!DirectoryExists(targetFolderPath))
        {   // if folder does not exist => create
            Warning("\nFolder for IRepo does not exist. \n>> Creating...");
            //target folder
            CreateDirectory(targetFolderPath);
            Information("\n>> Done!");
        }

        // - generate file
        Information($"\n>> Generate >> I{name}Repository.cs");  // log
        if (FileExists($"{targetFolderPath}/I{name}Repository.cs"))
        {   // check if file exists
            Warning($"\nError: I{name}Repository exists.");
        }
        else FileWriteText(  // fucntion: generate file + content
                        // parameters
                        // 1 - path/filename.extension
            $"{targetFolderPath}/I{name}Repository.cs",

            // 2 - content
            $@"using {name_space}.Core.Entities; 

namespace {name_space}.Contract
{{
	public interface I{name}Repository : IBaseRepository<{name}>
    {{
        
    }}
}}" // end content
        ); // end step
        #endregion

        #region [ 3. Repo ]
        // - setup path
        targetFolderPath = string.Format(FODLER_PATH_TEMPLATE, group_Repo, target_Repo);
        Information("\n* Target folder full path: " + targetFolderPath);

        if (!DirectoryExists(targetFolderPath))
        {   // if folder does not exist => create
            Warning("\nFolder for Repo does not exist. \n>> Creating...");
            //target folder
            CreateDirectory(targetFolderPath);
            Information("\n>> Done!");
        }

        // - generate file
        Information($"\n>> Generate >> {name}Repository.cs");   // log
        if (FileExists($"{targetFolderPath}/{name}Repository.cs"))
        {   // check if file exists
            Warning($"\nError: {name}Repository exists.");
        }
        else FileWriteText(  // fucntion: generate file + content
                        // parameters
                        // 1 - path/filename.extension
            $"{targetFolderPath}/{name}Repository.cs",

            // 2 - content
            $@"using {name_space}.Contract; using {name_space}.Core.Database; using {name_space}.Core.Entities;

namespace {name_space}.Repository
{{

	public class {name}Repository : BaseRepository<{name}>, I{name}Repository
	{{
		public {name}Repository(ApplicationDbContext context) : base(context) {{ }}

        
	}}
}}" // end content
        ); // end step
        #endregion

        #region [ 4. Controllers ]
        // - setup path
         targetFolderPath = string.Format(FODLER_PATH_TEMPLATE, group_Controllers, target_Controllers);
        Information("\n* Target folder full path: " + targetFolderPath);

        if (!DirectoryExists(targetFolderPath))
        {   // if folder does not exist => create
            Warning("\nFolder for Controllers does not exist. \n>> Creating...");
            //target folder
            CreateDirectory(targetFolderPath);
            Information("\n>> Done!");
        }

        // - generate file
        Information($"\n>> Generate >> {name}Controller.cs");   // log
        if (FileExists($"{targetFolderPath}/{name}Controller.cs"))
        {   // check if file exists
            Warning($"\nError: {name}Controller exists.");
        }
        else FileWriteText(  // fucntion: generate file + content
                        // parameters
                        // 1 - path/filename.extension
            $"{targetFolderPath}/{name}Controller.cs",

            // 2 - content
            $@"using AutoMapper; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore; using {name_space}.Contract; using {name_space}.Core.Database; using {name_space}.Core.Entities; using {name_space}.DTOs;

namespace {name_space}.Controllers
{{
    public class {name}Controller : BaseController
    {{
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _appDbContext;
        private readonly I{name}Repository _{name.ToLower()}Repository;
					
        public {name}Controller(	IMapper mapper,
							        ApplicationDbContext appDbContext,
							        I{name}Repository {name.ToLower()}Repository) {{
	        _mapper = mapper;
	        _appDbContext = appDbContext;
	        _{name.ToLower()}Repository = {name.ToLower()}Repository;
						
        }}
					
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {{
	        var {name.ToLower()}s = await _{name.ToLower()}Repository.FindAll().ToListAsync(cancellationToken);
	        return Ok(_mapper.Map<IEnumerable<{name}DTO>>({name.ToLower()}s));
        }}


        [HttpGet(""{{{name.ToLower()}Id}}"")]
        public async Task<IActionResult> GetById(int {name.ToLower()}Id, CancellationToken cancellationToken = default)
        {{
	        var {name.ToLower()} = await _{name.ToLower()}Repository.FindByIdAsync({name.ToLower()}Id, cancellationToken);
	        return {name.ToLower()} != null ? Ok(_mapper.Map<{name}DTO>({name.ToLower()})) : NotFound(""Unable to find the requested {name.ToLower()}""); 
        }}
					
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] {name}DTO dto, CancellationToken cancellationToken = default)
        {{
	        using var appTransaction = await _appDbContext.Database.BeginTransactionAsync();
	        var {name.ToLower()} = _mapper.Map<{name}>(dto);
	        if ({name.ToLower()} != null)
	        {{
		        _{name.ToLower()}Repository.Add({name.ToLower()});
		        await appTransaction.CommitAsync(cancellationToken);
		        return Ok({name.ToLower()}.Id);
	        }}
	        else return BadRequest(""Can't convert request to {name}"");
        }}
					
        [HttpPut(""{{id}}"")]
        public async Task<IActionResult> Update([FromBody] {name}DTO dto, int id, CancellationToken cancellationToken = default)
        {{
	        var {name} = await _{name.ToLower()}Repository.FindByIdAsync(id, cancellationToken);
	        if ({name} is null)
		        return NotFound();

	        _mapper.Map(dto, {name});
	        await _{name.ToLower()}Repository.SaveChangesAsync(cancellationToken);
	        return NoContent();
        }}

        [HttpDelete(""{{id}}"")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {{
	        var {name.ToLower()} = await _{name.ToLower()}Repository.FindByIdAsync(id, cancellationToken);
	        if ({name.ToLower()} is null)
		        return NotFound();

	        _{name.ToLower()}Repository.Delete({name.ToLower()});
						

	        await _{name.ToLower()}Repository.SaveChangesAsync(cancellationToken);
	        return NoContent();
        }}
    }}
}}" // end content
        ); // end step
        #endregion
    }

);

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget("SetupEntity");
