namespace Unity.Services.Economy.Models;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

/// <summary>
/// The base class for the more specific configuration types, e.g. CurrencyDefinition. These are used to define
/// the resources that you create in the Unity Dashboard.
/// </summary>
public class ConfigurationItemDefinition
{
    /// <summary>
    /// The configuration ID of the resource.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id;

    /// <summary>
    /// The name of the resource.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name;

    /// <summary>
    /// Resource type as it appears in the Unity dashboard.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type;

    /// <summary>
    /// Any custom data associated with this resource definition in a deserializable format.
    /// </summary>
    [JsonPropertyName("customData")]
    public object CustomDataDeserializable;

    /// <summary>
    /// The date this resource was created.
    /// </summary>
    [JsonPropertyName("created")]
    public EconomyDate Created;

    /// <summary>
    /// The date this resource was last modified.
    /// </summary>
    [JsonPropertyName("modified")]
    public EconomyDate Modified;
}

public class EconomyDate
{
    [JsonPropertyName("date")]
    public DateTime Date;
}

public class ConfigurationItemDefinitionList
{
    [JsonPropertyName("metadata")]
    public Metadata Metadata;

    [JsonPropertyName("results")]
    public List<ConfigurationItemDefinition> Results;
}
