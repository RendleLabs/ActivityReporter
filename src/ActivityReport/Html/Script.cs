namespace ActivityReport.Html
{
    public static class Script
    {
        public const string Js = @"
<script>
document.addEventListener('DOMContentLoaded', () => {
    for (const btn of document.querySelectorAll('.toggle-button')) {
      btn.addEventListener('click', (e) => {
        e.currentTarget.parentElement.parentElement.classList.toggle('collapsed');
      });
    }
  });
</script>
";
    }
}