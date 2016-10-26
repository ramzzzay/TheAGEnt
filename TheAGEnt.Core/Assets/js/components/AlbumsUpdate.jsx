import AutoComplete from "material-ui/AutoComplete";
import RaisedButton from "material-ui/RaisedButton";

import DropzoneComponent from "react-dropzone-component"
import Uploader from "./FileUploader";

module.exports = React.createClass({
    getInitialState: function () {
        return {
            albumName: "",
            albums: [],
        };
    },
    componentDidMount: function () {
        this.getAlbums();
    },
    getAlbums: function () {
        fetch("api/Photos/GetUserAlbumsNameById", {
            method: "GET",
            headers: new Headers({
                "Content-Type": "application/json",
                "Authorization": "bearer " + Cookie.load("tokenInfo")
            })
        })
            .then(r => r.json())
            .then(a => this.setState({albums: Array.from(a)}));
    },
    _albumNameFieldChange: function (text) {
        this.setState({albumName: text});
    },
    render: function () {
        return (
            <div className="Edit">
                <h3>Add You photos here!</h3>
                <AutoComplete onUpdateInput={this._albumNameFieldChange} onNewRequest={this._albumNameFieldChange}
                              floatingLabelText="showAllItems" filter={AutoComplete.fuzzyFilter} openOnFocus={true}
                              dataSource={this.state.albums}/><br/>
                <Uploader type="photos"
                          url="api/Photos/UploadUserImage"
                          changeState={this.changingPathToCard}
                          email={this.props.email}
                          albumName={this.state.albumName}/>
            </div>)
    }
});
