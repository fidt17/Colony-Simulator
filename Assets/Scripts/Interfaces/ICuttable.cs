public interface ICuttable {
    bool HasCutJob { get; set; }
    Item Cut();
    void HandleCutJobResult(object job, System.EventArgs e);
}