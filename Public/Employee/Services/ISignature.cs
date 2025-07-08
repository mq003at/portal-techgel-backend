using portal.DTOs;

namespace portal.Services;

public interface ISignatureService
{
    /// <summary>
    /// Uploads an SVG signature for the given employee:
    ///   - Validates EmployeeId exists
    ///   - Generates a unique filename
    ///   - SFTPs the file to files.quan-ng.uk/erp/image-signatures/
    ///   - Persists a Signature record
    /// Returns the created SignatureDTO.
    /// </summary>
    Task<SignatureDTO> UploadAsync(UploadSignatureDTO dto);
    Task<SignatureDTO> UploadAndReplaceAsync(UploadSignatureDTO dto);
    Task<SignatureDTO?> GetByEmployeeAsync(int employeeId);
    Task DeleteSignatureAsync(int employeeId);
    Task<Stream> GetSignatureStreamAsync(int employeeId);
}
