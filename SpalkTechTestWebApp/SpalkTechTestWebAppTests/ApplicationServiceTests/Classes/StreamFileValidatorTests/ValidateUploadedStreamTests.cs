using FluentAssertions;
using SpalkTechTestWebApp.ApplicationService.Classes;
using SpalkTechTestWebApp.ApplicationService.Interfaces;

namespace SpalkTechTestWebAppTests.ApplicationServiceTests.Classes.StreamFileValidatorTests
{
    public class ValidateUploadedStreamTests
    {
        private readonly IStreamFileValidator _streamFileValidator = new StreamFileValidator();

        private const int Seed = 12345;

        public ValidateUploadedStreamTests(){}

        [Fact]
        public void Should_ReturnSuccessMessage_With_PIDStrings_When_StreamIsValid()
        {
            // Given
            var validByteStream = CreateTestByteStream(firstSyncByte: 0x47, secondSyncByte: 0x47);

            // When
            var response =
                _streamFileValidator.ValidateUploadedStream(new MemoryStream(validByteStream));

            // Then
            response.IsStreamValid.Should().BeTrue();
            response.ErrorMessage.Should().BeNull();
            response.SuccessMessage.Should().Be("0x1F7F" + "\n" + "0x1FFF" + "\n");
        }

        [Fact]
        public void Should_ReturnErrorMessage_With_MissingSyncByteInformation_When_StreamIsInvalid()
        {
            // Given
            var invalidByteStream = CreateTestByteStream(firstSyncByte: 0x47, secondSyncByte: 0x48);

            // When
            var response =
                _streamFileValidator.ValidateUploadedStream(new MemoryStream(invalidByteStream));

            // Then
            response.IsStreamValid.Should().BeFalse();
            response.SuccessMessage.Should().BeNull();
            response.ErrorMessage.Should().Be($"No sync byte present in packet 1, offset 188");
        }

        [Fact]
        public void Should_ReturnSuccessMessage_With_PIDStringsForValidPackets_When_FirstPacketMissesSyncByte() {

            // Given
            var validByteStream = CreateTestByteStream(firstSyncByte: 0x48, secondSyncByte: 0x47);

            // When
            var response = _streamFileValidator.ValidateUploadedStream(new MemoryStream(validByteStream));

            // Then
            response.IsStreamValid.Should().BeTrue();
            response.ErrorMessage.Should().BeNull();
            response.SuccessMessage.Should().Be("0x1F7F" + "\n");
        }


        private static byte[] CreateTestByteStream(byte firstSyncByte, byte secondSyncByte)
        {

            byte[] byteStream = new byte[376];

            Random random = new(Seed);

            random.NextBytes(byteStream);

            byteStream[0] = firstSyncByte;
            byteStream[1] = 0xFF; // PID bytes
            byteStream[2] = 0xFF;

            byteStream[188] = secondSyncByte;
            byteStream[189] = 0XFF; // PID bytes
            byteStream[190] = 0X7F;

            return byteStream;
        }
    }
}

