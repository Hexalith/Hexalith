// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Domain.Abstractions.Messages;

/// <summary>
/// Base interface for messages.
/// </summary>
public interface IMessage
{
	/// <summary>
	/// Gets the domain aggregate id.
	/// </summary>
	string AggregateId { get; }

	/// <summary>
	/// Gets the domain aggregate name.
	/// </summary>
	string AggregateName { get; }

	/// <summary>
	/// Gets the message major version
	/// </summary>
	int MajorVersion { get; }

	/// <summary>
	/// Gets the message name
	/// </summary>
	string MessageName { get; }

	/// <summary>
	/// Gets the message minor version
	/// </summary>
	int MinorVersion { get; }
}