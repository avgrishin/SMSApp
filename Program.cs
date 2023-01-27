﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;

var sms = new
{
  messages = new[] { new { content = new { short_text = "SMS Text Привет" }, to = new[] { new { msisdn = "79160764810" } } } },
  options = new { from = new { sms_address = "MTSM_Test" } }
};
string jsonString = JsonSerializer.Serialize(sms);
Console.WriteLine(jsonString);
HttpClient hc = new HttpClient();
var message = new HttpRequestMessage(HttpMethod.Post, "https://omnichannel.mts.ru/http-api/v1/messages");
message.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("gw_Y4LG3GBeubor:XfBxSNTc")));
message.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
var response = await hc.SendAsync(message);
response.EnsureSuccessStatusCode();
var s = await response.Content.ReadFromJsonAsync<MessagesRoot>();
Console.WriteLine(s);

var message1 = new HttpRequestMessage(HttpMethod.Post, "https://omnichannel.mts.ru/http-api/v1/messages/info");
message1.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("gw_Y4LG3GBeubor:XfBxSNTc")));
message1.Content = new StringContent($"{{\"int_ids\": [\"{s?.messages[0].internal_id}\"]}}", Encoding.UTF8, "application/json");
response = await hc.SendAsync(message1);
response.EnsureSuccessStatusCode();
var s1 = await response.Content.ReadFromJsonAsync<EventRoot>();
Console.WriteLine(s1);

public class MessagesRoot
{
  public Messages[] messages { get; set; }
}
public class Messages
{
  public string internal_id { get; set; } = string.Empty; public string msisdn { get; set; } = string.Empty;
}
public class EventRoot
{
  public EventInfo[] events_info { get; set; }
}
public class EventInfo
{
  public EventsDetails[] events_info { get; set; }
  public string key { get; set; }
}
public class EventsDetails
{
  public string call_direction { get; set; }
  public int client_id { get; set; }
  public DateTime event_at { get; set; }
  public int[]? internal_errors { get; set; }
  public string internal_id { get; set; }
  public DateTime received_at { get; set; }
  public int status { get; set; }
  public int total_parts { get; set; }
}