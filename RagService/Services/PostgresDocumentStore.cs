using Dapper;
using Npgsql;
using RagService.API.Models;
using RagService.Services;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace RagService.API.Services
{
    public class PostgresDocumentStore : IDocumentStore
    {
        private readonly string _conn;

        public PostgresDocumentStore(string connectionString)
        {
            _conn = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<DocumentDto>> GetAllDocumentsAsync(CancellationToken ct)
        {
            var sql = "SELECT id, source_id AS sourceid, source_type AS sourcetype, content, embedding, created_at as createdat FROM rag_documents";
            await using var conn = new NpgsqlConnection(_conn);
            await conn.OpenAsync(ct);

            try
            {
                var rows = await conn.QueryAsync(sql);
                var list = rows.Select(x=> {
                    var embJson = (string)x.embedding;
                    float[]? emb = null;
                    if (!string.IsNullOrEmpty(embJson)) emb = JsonSerializer.Deserialize<float[]>(embJson);
                    return new DocumentDto((int?)(x.id) ?? 0, (string)x.sourceid, (string)x.sourcetype, (string)x.content, emb, (DateTime)x.createdat);
                }).ToList();

                return list;
            }
            catch (Exception ex)
            {
                var res = ex.InnerException.Message;
                return null;
            }
            finally { await conn.CloseAsync(); }
        }

        async Task<DocumentDto?> IDocumentStore.GetDocumentByIdAsync(int id, CancellationToken ct)
        {
            var sql = "SELECT id, source_id AS sourceid, source_type AS sourcetype, content, embedding, created_at as createdat FROM rag_documents where id = @id";
            await using var conn = new NpgsqlConnection(_conn);
            await conn.OpenAsync(ct);

            try
            {
                var x = await conn.QuerySingleOrDefault(sql, new { id});
                if (x == null) return null;
                var embjson = (string)x.embedding;
                float[]? emb = null;
                if (!string.IsNullOrEmpty(embjson)) emb = JsonSerializer.Deserialize<float[]>(embjson);
                return new DocumentDto((int?)(x.id) ?? 0, (string)x.sourceid, (string)x.sourcetype, (string)x.content, emb, (DateTime)x.createdat);

            }
            catch (Exception ex)
            {
                var res = ex.InnerException.Message;
                return null;
            }
            finally { await conn.CloseAsync(); }
        }

        public async Task<int> InsertDocumentAsync(string sourceId, string sourceType, string content, float[] embedding, CancellationToken ct)
        {
            var embJson = JsonSerializer.Serialize(embedding);
            await using var conn = new NpgsqlConnection(_conn);
            var sql = @"Insert Into rag_documents(source_id, source_type, content, embedding)
                        Values(@sid, @stype, @cnt, @emb::jsonb) Returning id";

            await conn.OpenAsync();
            try
            {
                var id = conn.ExecuteScalar<int>(sql, new {sid=sourceId, stype=sourceType, cnt = content, emb = embJson });
                return id;
            }
            catch (Exception ex)
            {
                var res = ex.InnerException.Message;
                return 0;
            }
            finally { await conn.CloseAsync(); }
        }

        public async Task<int> InsertNoteAsync(int patientId, string note, float[] embedding)
        {
            var sql = @"Insert Into patient_notes(patient_id, note, embedding)
                        Values(@pid, @note, @emb::jsonb) Returning id";

            await using var conn = new NpgsqlConnection(_conn);

            await conn.OpenAsync();
            try
            {
                var embJson = JsonSerializer.Serialize(embedding);
                var id = await conn.ExecuteScalarAsync<int>(sql, new { pid = patientId, note, emb = embJson });
                return id;
            }
            catch (Exception ex)
            {
                var res = ex.InnerException.Message;
                return 0;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public async Task<IEnumerable<PatientNote>> GetAllNotesAsync(int patientId)
        {
            var sql = @"Select id, patient_id as patientid, note, embedding, created_at as createdat 
                        from patient_notes where patient_id = @pid";

            await using var conn = new NpgsqlConnection(_conn);
            await conn.OpenAsync();
            try
            {
                var rows = await conn.QueryAsync(sql, new { pid = patientId });
                var list = rows.Select(x =>
                {
                    var embJson = (string)x.embedding;
                    float[]? emb = null;
                    if (!string.IsNullOrEmpty(embJson)) emb = JsonSerializer.Deserialize<float[]>(embJson);
                    return new PatientNote((int?)(x.id) ?? 0, (int?)(x.patientid) ?? 0, (string)x.note, emb, (DateTime)x.createdat);
                }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                var res = ex.InnerException.Message;
                return null;
            }
            finally
            {
                await conn.CloseAsync();

            }
        }
    }
}
