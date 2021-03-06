/* 
 * Thermonuclear War REST API
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace IO.Swagger.Model
{
    /// <summary>
    /// WarheadLaunchResult
    /// </summary>
    [DataContract]
    public partial class WarheadLaunchResult :  IEquatable<WarheadLaunchResult>, IValidatableObject
    {
        /// <summary>
        /// Gets or Sets Result
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ResultEnum
        {
            
            /// <summary>
            /// Enum Success for "Success"
            /// </summary>
            [EnumMember(Value = "Success")]
            Success,
            
            /// <summary>
            /// Enum Failure for "Failure"
            /// </summary>
            [EnumMember(Value = "Failure")]
            Failure
        }

        /// <summary>
        /// Gets or Sets Result
        /// </summary>
        [DataMember(Name="Result", EmitDefaultValue=false)]
        public ResultEnum? Result { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="WarheadLaunchResult" /> class.
        /// </summary>
        /// <param name="Result">Result.</param>
        /// <param name="Message">Message.</param>
        public WarheadLaunchResult(ResultEnum? Result = default(ResultEnum?), string Message = default(string))
        {
            this.Result = Result;
            this.Message = Message;
        }
        
        /// <summary>
        /// Gets or Sets Message
        /// </summary>
        [DataMember(Name="Message", EmitDefaultValue=false)]
        public string Message { get; set; }
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class WarheadLaunchResult {\n");
            sb.Append("  Result: ").Append(Result).Append("\n");
            sb.Append("  Message: ").Append(Message).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as WarheadLaunchResult);
        }

        /// <summary>
        /// Returns true if WarheadLaunchResult instances are equal
        /// </summary>
        /// <param name="other">Instance of WarheadLaunchResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(WarheadLaunchResult other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Result == other.Result ||
                    this.Result != null &&
                    this.Result.Equals(other.Result)
                ) && 
                (
                    this.Message == other.Message ||
                    this.Message != null &&
                    this.Message.Equals(other.Message)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)
                if (this.Result != null)
                    hash = hash * 59 + this.Result.GetHashCode();
                if (this.Message != null)
                    hash = hash * 59 + this.Message.GetHashCode();
                return hash;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        { 
            yield break;
        }
    }

}
