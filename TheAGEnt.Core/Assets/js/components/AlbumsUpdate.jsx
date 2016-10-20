import AutoComplete from "material-ui/AutoComplete";
import RaisedButton from "material-ui/RaisedButton";
import DropzoneComponent from "react-dropzone-component"

module.exports = React.createClass({
    getInitialState: function () {
        return {
            albumName: "",
            albums: [],
            componentConfig: {
                postUrl: "api/Photos/UploadUserImage"
            },
            djsConfig: {
                addRemoveLinks: true,
                headers: {
                    "Authorization": "bearer " + Cookie.load("tokenInfo")
                }
            },
            eventHandlers: {
                sending: (file, xhr, formData) => {
                    formData.append("Email", this.props.email);
                    formData.append("Album", this.state.albumName);
                },
                success: (file, response) => this.getAlbums()
            }
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
                <DropzoneComponent config={this.state.componentConfig} eventHandlers={this.state.eventHandlers}
                                   djsConfig={this.state.djsConfig}/>
                <RaisedButton label="Ok, save my new info!" primary={true} onClick={this.sendToServer}/>
            </div>)
    }
});
